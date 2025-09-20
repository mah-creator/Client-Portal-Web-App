import AlertIcon from "../../icons/AlertIcon";
import ChartIcon from "../../icons/ChartIcon";
import CheckIcon from "../../icons/CheckIcon";
import TrendUpIcon from "../../icons/TrendUpIcon";
import StatCard from "./StatCard";

function StatCards({ stats }) {
  return (
    <section className="gap-4 grid sm:grid-cols-2 lg:grid-cols-4">
      <StatCard
        title="Active Projects"
        value={stats.activeProjects}
        icon={<ChartIcon />}
      />
      <StatCard
        title="Tasks Completed"
        value={stats.tasksCompleted}
        icon={<CheckIcon />}
      />
      <StatCard
        title="Pending Tasks"
        value={stats.pendingTasks}
        icon={<AlertIcon />}
      />
      <StatCard
        title="This Month"
        value={`$${Intl.NumberFormat().format(stats.revenueThisMonth)}`}
        icon={<TrendUpIcon />}
      />
    </section>
  );
}

export default StatCards;
