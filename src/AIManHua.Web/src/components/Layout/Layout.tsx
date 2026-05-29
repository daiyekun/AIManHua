import { Outlet, NavLink } from "react-router-dom";

export default function Layout() {
  return (
    <div className="app-layout">
      <nav className="navbar">
        <h1 className="logo">AI 漫画生成</h1>
        <div className="nav-links">
          <NavLink to="/">创建漫画</NavLink>
          <NavLink to="/history">历史记录</NavLink>
        </div>
      </nav>
      <main className="main-content">
        <Outlet />
      </main>
    </div>
  );
}
