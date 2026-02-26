import { useEffect } from "react";
import "../style/Sidebar.css";

type SidebarLink = {
  label: string;
  path: string;
  icon: string;
};

const links: SidebarLink[] = [
  { label: "Appointments", path: "/appointments", icon: "🗓️" },
  { label: "Patients", path: "/patients", icon: "🧑‍⚕️" },
  { label: "Doctors", path: "/doctors", icon: "👨‍⚕️" },
];

type SidebarProps = {
  isOpen: boolean;
  onClose: () => void;
  activePath?: string;
};

const Sidebar = ({ isOpen, onClose, activePath }: SidebarProps) => {
  useEffect(() => {
    const handleKey = (e: KeyboardEvent) => {
      if (e.key === "Escape") onClose();
    };
    document.addEventListener("keydown", handleKey);
    return () => document.removeEventListener("keydown", handleKey);
  }, [onClose]);

  return (
    <>
      <div
        className={`sidebar-overlay ${isOpen ? "sidebar-overlay--visible" : ""}`}
        onClick={onClose}
      />

      <div className={`sidebar ${isOpen ? "sidebar--open" : ""}`}>
        <div className="sidebar__header">
          <span className="sidebar__logo-text">MediApp</span>
          <button className="sidebar__close" onClick={onClose}>✕</button>
        </div>

        <nav className="sidebar__nav">
          {links.map((link) => (
            <a
              key={link.path}
              href={link.path}
              className={`sidebar__link ${activePath === link.path ? "sidebar__link--active" : ""}`}
              onClick={onClose}
            >
              <span className="sidebar__link-icon">{link.icon}</span>
              {link.label}
            </a>
          ))}
        </nav>

        <div className="sidebar__footer">
          <p className="sidebar__footer-text">v1.0.0</p>
        </div>
      </div>
    </>
  );
};

export default Sidebar;