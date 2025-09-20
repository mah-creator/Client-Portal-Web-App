import BellIcon from "../icons/BellIcon";

export default function TopBar({
  name = "User",
  role = "freelancer",
  initials = "US",
}) {
  return (
    <div className="bg-white/60 backdrop-blur border-slate-200 border-b w-full">
      <div className="flex justify-between items-center mx-auto px-6 py-3 max-w-7xl">
        <div className="h-6" />
        <div className="flex items-center gap-4">
          <button className="relative place-items-center grid bg-slate-100 hover:bg-slate-200 rounded-full w-9 h-9 text-slate-700">
            <BellIcon />
            <span className="top-1 right-1 absolute bg-rose-500 rounded-full w-2 h-2" />
          </button>
          <div className="flex items-center gap-3">
            <div className="place-items-center grid bg-slate-200 rounded-full w-9 h-9 font-medium text-slate-700 text-xs">
              {initials}
            </div>
            <div className="leading-tight">
              <div className="font-medium text-sm">{name}</div>
              <div className="text-slate-500 text-xs">{role}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
