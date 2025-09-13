import { Outlet } from "react-router";

function AppLayout() {
  return (
    <div className="w-full h-dvh">
      <Outlet />
    </div>
  );
}

export default AppLayout;
