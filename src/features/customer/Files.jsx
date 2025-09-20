import FileRow from "./FileRow";

function Files({ data }) {
  return (
    <div>
      <h3 className="mb-4 font-semibold text-xl">Recent Files</h3>
      <div className="bg-white shadow p-2 border border-slate-200 rounded-2xl">
        {data.files.map((f, i) => (
          <FileRow key={f.id} {...f} last={i === data.files.length - 1} />
        ))}
      </div>
    </div>
  );
}

export default Files;
