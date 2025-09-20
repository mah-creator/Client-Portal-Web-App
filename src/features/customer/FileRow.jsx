import DocIcon from "../../icons/DocIcon";
import DownloadIcon from "../../icons/DownloadIcon";

function FileRow({ icon = <DocIcon />, name, meta, last }) {
  return (
    <div
      className={`flex items-center justify-between gap-4 p-3 ${
        last ? "" : "border-b border-slate-100"
      }`}
    >
      <div className="flex items-center gap-4">
        <div className="place-items-center grid bg-blue-50 rounded-xl w-12 h-12 text-blue-600">
          {icon}
        </div>
        <div>
          <p className="font-medium">{name}</p>
          <p className="text-slate-500 text-sm">{meta}</p>
        </div>
      </div>
      <button
        className="place-items-center grid hover:bg-slate-100 rounded-lg w-9 h-9 text-slate-700"
        title="Download"
      >
        <DownloadIcon />
      </button>
    </div>
  );
}

export default FileRow;
