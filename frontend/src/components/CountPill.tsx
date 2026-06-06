interface CountPillProps {
  count: number;
}

export default function CountPill({ count }: CountPillProps) {
  return <span className="pill">{count}</span>;
}
