import { Badge } from "@/components/ui/badge";
import type { AssetStatus } from "@/lib/api";

const STATUS_CONFIG: Record<
  AssetStatus,
  { label: string; variant: "success" | "default" | "warning" | "outline" }
> = {
  Available: { label: "Available", variant: "success" },
  InUse: { label: "In use", variant: "default" },
  Maintenance: { label: "Maintenance", variant: "warning" },
  Retired: { label: "Retired", variant: "outline" },
};

type AssetStatusBadgeProps = {
  status: AssetStatus;
};

export const AssetStatusBadge = ({ status }: AssetStatusBadgeProps) => {
  const { label, variant } = STATUS_CONFIG[status];

  return <Badge variant={variant}>{label}</Badge>;
};
