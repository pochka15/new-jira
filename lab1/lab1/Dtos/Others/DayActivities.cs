using System.Collections.Generic;
using lab1.Models;

namespace lab1.Dtos.Others {
// TODO(@pochka15): MEH name: e.x. usage: dayActivities.activities 😕 
public class DayActivities {
    public bool Frozen { get; set; }
    public List<Activity> Activities { get; set; }
}
}