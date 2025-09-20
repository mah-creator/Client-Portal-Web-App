function ActivityItem({ text, time }) {
  return (
    <div className="flex items-start gap-3 hover:bg-slate-50 p-3 rounded-xl">
      <span className="bg-blue-500 mt-2 rounded-full w-2 h-2" />
      <div>
        <p className="text-sm">{text}</p>
        <p className="text-slate-500 text-xs">{time}</p>
      </div>
    </div>
  );
}

export default ActivityItem;
