import CalendarIcon from "../../icons/CalendarIcon";
import ChatIcon from "../../icons/ChatIcon";
import DownloadIcon from "../../icons/DownloadIcon";
import ActionRow from "./ActionRow";

function QuickActions() {
  return (
    <div>
      <h3 className="mb-4 font-semibold text-xl">Quick Actions</h3>
      <div className="bg-white shadow p-2 border border-slate-200 rounded-2xl">
        <ActionRow icon={<ChatIcon />} label="Send Feedback" />
        <ActionRow icon={<DownloadIcon />} label="Download All Files" />
        <ActionRow icon={<CalendarIcon />} label="Schedule Meeting" last />
      </div>
    </div>
  );
}

export default QuickActions;
