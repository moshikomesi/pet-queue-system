export function getUserIdFromToken(): number | null {
  const token = localStorage.getItem("token");
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));

    const userId =
      payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

    return userId ? Number(userId) : null;
  } catch {
    return null;
  }
}
