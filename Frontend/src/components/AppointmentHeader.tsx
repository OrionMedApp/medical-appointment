import BurgerMenu from "./BurgerMenu";
import "../style/Header.css";

type AppointmentHeaderProps = {
  isSidebarOpen: boolean;
  onToggleSidebar: () => void;
};

const AppointmentHeader = ({ isSidebarOpen, onToggleSidebar }: AppointmentHeaderProps) => {
  return (
    <div className="appointments-header">
      <div className="appointments-header__left">
        <BurgerMenu isOpen={isSidebarOpen} onToggle={onToggleSidebar} />
      </div>

      <div className="appointments-header__center">
        <h2 className="appointments-header__title">Appointments</h2>
        <p className="appointments-header__subtitle">Overview & scheduling</p>
      </div>

      <div className="appointments-header__right">
        <div className="appointments-header__logo-placeholder">
         <img 
            src="/image.png" 
            alt="Company logo" 
            className="appointments-header__logo"
            onError={(e) => console.log("Logo se nije učitao")} 
            />
        </div>
      </div>
    </div>
  );
};

export default AppointmentHeader;