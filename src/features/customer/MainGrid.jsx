import Files from "./Files";
import Notifications from "./Notifications";
import Projects from "./Projects";
import QuickActions from "./QuickActions";

function MainGrid({ data }) {
  return (
    <section className="items-start gap-6 grid lg:grid-cols-3">
      <div className="space-y-6 lg:col-span-2">
        <Projects data={data} />
        <Files data={data} />
      </div>
      <aside className="space-y-6">
        <Notifications data={data} />
        <QuickActions />
      </aside>
    </section>
  );
}

export default MainGrid;
