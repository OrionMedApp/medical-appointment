import "../style/footer.css";

const AppFooter = () => {
  const year = new Date().getFullYear();

  return (
    <footer className="app-footer">
      <div className="app-footer__content">
        <p className="app-footer__text">
          © {year} Internship Group D — Powered by interns and optimism.
        </p>
        <div className="app-footer__links">
          <span>Privacy Policy</span>
          <span className="app-footer__dot">·</span>
          <span>Terms of Use</span>
          <span className="app-footer__dot">·</span>
          <span>Support</span>
        </div>
      </div>
    </footer>
  );
};

export default AppFooter;