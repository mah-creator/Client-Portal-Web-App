function ActionRow({ icon, label, last }) {
  return (
    <button
      className={`flex w-full items-center gap-3 rounded-xl px-4 py-3 text-left hover:bg-slate-50 ${
        last ? "" : "border-b border-slate-100"
      }`}
      type="button"
    >
      <span className="place-items-center grid bg-slate-100 rounded-lg w-8 h-8 text-slate-700">
        {icon}
      </span>
      <span className="font-medium text-sm">{label}</span>
    </button>
  );
}

export default ActionRow;
