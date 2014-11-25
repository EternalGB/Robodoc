using UnityEngine;
using System.Collections.Generic;

public class TutorialEventManager : MonoBehaviour
{

	public List<TutorialEvent> events;
	public bool running;
	public bool Finished
	{
		get; private set;
	}

	Queue<TutorialEvent> eventQueue;
	List<TutorialEvent> currentEvents;
	int currentPriority;
	
	public void BeginTutorial()
	{
		events.Sort((e1,e2) => e1.priority - e2.priority);
		eventQueue = new Queue<TutorialEvent>(events);
		NextPriority();
		running = true;
	}

	void Update()
	{
		if(running){
			bool completed = true;
			foreach(TutorialEvent te in currentEvents) {
				completed &= te.Completed();
			}
			if(completed) {
				CleanUpEvents(currentEvents);
				NextPriority();
			}
		}
	}

	void LaunchEvents(List<TutorialEvent> events)
	{
		foreach(TutorialEvent te in events) {
			te.Activate();
		}
	}

	void CleanUpEvents(List<TutorialEvent> events)
	{
		foreach(TutorialEvent te in events) {
			te.Deactivate();
		}
	}

	void NextPriority()
	{
		if(eventQueue.Count > 0) {
			currentEvents = new List<TutorialEvent>();
			currentPriority = eventQueue.Peek().priority;
			while(eventQueue.Count > 0 && eventQueue.Peek().priority == currentPriority) {
				currentEvents.Add(eventQueue.Dequeue());
			}
			LaunchEvents(currentEvents);
		} else {
			running = false;
			Finished = true;
		}
	}

}

