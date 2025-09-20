function StatCard({ title, value, icon }) {
  return (
    <div className="flex justify-between items-center bg-white shadow-sm p-5 border border-slate-200 rounded-2xl">
      <div>
        <p className="text-slate-500 text-sm">{title}</p>
        <p className="mt-1 font-semibold text-2xl">{value}</p>
      </div>
      <div className="place-items-center grid bg-blue-50 rounded-xl w-10 h-10 text-blue-600">
        {icon}
      </div>
    </div>
  );
}

export default StatCard;
