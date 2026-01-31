import { Link } from "react-router-dom";

type Props = {
  children: React.ReactNode;
};

export default function MainLayout({ children }: Props) {
  return (
    <div style={pageStyle}>
      <header style={headerStyle}>
        <h2>PetQueue</h2>
        <nav style={navStyle}>
          <Link to="/">Home</Link>
          <Link to="/login">Login</Link>
        </nav>
      </header>

      <main style={contentStyle}>{children}</main>
    </div>
  );
}

/* ---------- styles ---------- */

const pageStyle: React.CSSProperties = {
  minHeight: "100vh",
  background: "#f5f7fa",
};

const headerStyle: React.CSSProperties = {
  background: "#1e293b",
  color: "white",
  padding: "12px 24px",
  display: "flex",
  justifyContent: "space-between",
  alignItems: "center",
};

const navStyle: React.CSSProperties = {
  display: "flex",
  gap: 16,
};

const contentStyle: React.CSSProperties = {
  padding: 24,
};
