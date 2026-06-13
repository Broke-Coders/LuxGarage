export const STATUS_MAP = {
  1: { label: "Available",       css: "available" },
  2: { label: "Rented",          css: "rented" },
  3: { label: "Maintenance",     css: "maintenance" },
  4: { label: "Out of Service",  css: "outofservice" },
  5: { label: "Retired",         css: "retired" },
};

export function statusBadge(statusStr) {
  const entry = Object.values(STATUS_MAP).find(
    (s) => s.label.toLowerCase() === statusStr?.toLowerCase()
  );
  if (!entry) return `<span class="status-badge">${statusStr ?? "—"}</span>`;
  return `<span class="status-badge status-${entry.css}">${entry.label}</span>`;
}

export function statusValueFromLabel(label) {
  const entry = Object.entries(STATUS_MAP).find(
    ([, v]) => v.label.toLowerCase() === label?.toLowerCase()
  );
  return entry ? parseInt(entry[0]) : 1;
}