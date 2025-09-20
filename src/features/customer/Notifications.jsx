import NotificationItem from "./NotificationItem";

function Notifications({ data }) {
  return (
    <div>
      <h3 className="mb-4 font-semibold text-xl">Notifications</h3>
      <div className="bg-white shadow p-4 border border-slate-200 rounded-2xl">
        {data.notifications.map((n) => (
          <NotificationItem key={n.id} {...n} />
        ))}
      </div>
    </div>
  );
}

export default Notifications;
