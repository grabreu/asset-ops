import { useQuery } from "@tanstack/react-query";
import { createFileRoute } from "@tanstack/react-router";
import { AssetsStatusSummary } from "@/features/assets/components/AssetsStatusSummary";
import { AssetsTable } from "@/features/assets/components/AssetsTable";
import { CreateAssetDialog } from "@/features/assets/components/CreateAssetDialog";
import { listAssetsOptions } from "@/lib/api/@tanstack/react-query.gen";

const AssetsListPage = () => {
  const { data, isPending, isError } = useQuery(listAssetsOptions());

  if (isPending) {
    return <p className="text-muted-foreground">Loading assets…</p>;
  }

  if (isError) {
    return (
      <p className="text-destructive">
        Couldn't load assets. Try refreshing the page.
      </p>
    );
  }

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-lg font-semibold">Assets</h1>
        <CreateAssetDialog />
      </div>

      <AssetsStatusSummary assets={data} />
      <AssetsTable assets={data} />
    </div>
  );
};

export const Route = createFileRoute("/_app/")({
  component: AssetsListPage,
});
