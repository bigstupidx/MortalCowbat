using Ge;
using System.Collections.Generic;


namespace Ge
{
	public class WaveManager
	{
		readonly Wave wave;

		public WaveManager(Wave wave)
		{
			this.wave = wave;
		}

		public List<Wave.Event> GetDependentEventsOnEvent(int id, Wave.Event.Condition trigger)
		{
			var evt = FindEvent(id);
			return evt.DependentEvents.FindAll(x=>!x.RuntimeData.Processed && x.Trigger == trigger);
		}

		public Wave.Event FindEvent(int id)
		{
			var  eventList = new Queue<Wave.Event>(wave.Events);
			while (eventList.Count > 0) {
				var firstEvent = eventList.Dequeue();
				if (firstEvent.RuntimeData.Processed && firstEvent.RuntimeData.Id == id)
					return firstEvent;

				for (int i = 0; i < firstEvent.DependentEvents.Count; ++i) {
					eventList.Enqueue(firstEvent.DependentEvents[i]);
				}
			}
			return null;
		}

		public int GetTotalNotProcessedEventsCount()
		{
			int notProcessedEventCount = 0;
			for (int i = 0; i < wave.Events.Count; ++i) {
				GetNotProcessedEvents(wave.Events[i], ref notProcessedEventCount);
			}
			return notProcessedEventCount;
		}

		void GetNotProcessedEvents(Wave.Event evt, ref int notProcessedEventCount)
		{
			if (!evt.RuntimeData.Processed) {
				notProcessedEventCount++;
			}
			for (int i = 0; i < evt.DependentEvents.Count; ++i) {
				GetNotProcessedEvents(evt.DependentEvents[i], ref notProcessedEventCount);
			}
		}

		public List<Wave.Event> GetTimePassedEvents (float globalTime)
		{
			return wave.Events.FindAll(x => x.Time < globalTime && !x.RuntimeData.Processed);
		}
	}
}

