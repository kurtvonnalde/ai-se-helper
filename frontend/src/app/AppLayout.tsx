import { Outlet } from "react-router-dom";
import "../App.css";

export default function AppLayout() {
  return (
    <div className="app-shell">
      <div className="ambient ambient-one" aria-hidden="true" />
      <div className="ambient ambient-two" aria-hidden="true" />
      <Outlet />
    </div>
  );
}