import { useMemo } from "react";
import type { AssetStatus, AssetSummaryDto } from "@/lib/api";

const STATUS_ORDER: AssetStatus[] = [
  "Available",
  "InUse",
  "Maintenance",
  "Retired",
];

const STATUS_LABELS: Record<AssetStatus, string> = {
  Available: "Available",
  InUse: "In use",
  Maintenance: "Maintenance",
  Retired: "Retired",
};

type AssetsStatusSummaryProps = {
  assets: AssetSummaryDto[];
};

export const AssetsStatusSummary = ({ assets }: AssetsStatusSummaryProps) => {
  const counts = useMemo(() => {
    const initial: Record<AssetStatus, number> = {
      Available: 0,
      InUse: 0,
      Maintenance: 0,
      Retired: 0,
    };

    return assets.reduce((acc, asset) => {
      acc[asset.status] += 1;
      return acc;
    }, initial);
  }, [assets]);

  return (
    <dl className="grid grid-cols-2 gap-3 sm:grid-cols-4">
      {STATUS_ORDER.map((status) => (
        <div
          key={status}
          className="rounded-lg border bg-card p-3 text-card-foreground"
        >
          <dt className="text-xs text-muted-foreground">
            {STATUS_LABELS[status]}
          </dt>
          <dd className="text-2xl font-semibold">{counts[status]}</dd>
        </div>
      ))}
    </dl>
  );
};
