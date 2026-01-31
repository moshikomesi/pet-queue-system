export function formatDateTime(date: string | Date): string {
  const d = typeof date === "string" ? new Date(date) : date;

  const dd = String(d.getDate()).padStart(2, "0");
  const mm = String(d.getMonth() + 1).padStart(2, "0");
  const yy = String(d.getFullYear()).slice(-2);

  const hh = String(d.getHours()).padStart(2, "0");
  const min = String(d.getMinutes()).padStart(2, "0");

  return `${dd}/${mm}/${yy} ${hh}:${min}`;
}

export function getNowForDateTimeLocal(selected?: string): string | undefined {
  const now = new Date();

  const pad = (n: number) => String(n).padStart(2, "0");

  const today = `${now.getFullYear()}-${pad(
    now.getMonth() + 1
  )}-${pad(now.getDate())}`;

  if (!selected) {
    return `${today}T${pad(now.getHours())}:${pad(now.getMinutes())}`;
  }

  if (selected.startsWith(today)) {
    return `${today}T${pad(now.getHours())}:${pad(now.getMinutes())}`;
  }

  return undefined;
}


