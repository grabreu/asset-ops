import {
  columnFilteringFeature,
  createColumnHelper,
  createFilteredRowModel,
  createPaginatedRowModel,
  filterFn_equalsString,
  filterFn_includesString,
  globalFilteringFeature,
  rowPaginationFeature,
  tableFeatures,
  useTable,
} from "@tanstack/react-table";
import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { AssetStatusBadge } from "@/features/assets/components/AssetStatusBadge";
import type { AssetStatus, AssetSummaryDto } from "@/lib/api";

const features = tableFeatures({
  columnFilteringFeature,
  globalFilteringFeature,
  rowPaginationFeature,
  filteredRowModel: createFilteredRowModel(),
  paginatedRowModel: createPaginatedRowModel(),
  filterFns: {
    includesString: filterFn_includesString,
    equalsString: filterFn_equalsString,
  },
});

const columnHelper = createColumnHelper<typeof features, AssetSummaryDto>();

const dateFormatter = new Intl.DateTimeFormat("en-US", { dateStyle: "medium" });

const columns = columnHelper.columns([
  columnHelper.accessor("tag", { header: "Tag" }),
  columnHelper.accessor("name", { header: "Name" }),
  columnHelper.accessor("status", {
    header: "Status",
    filterFn: "equalsString",
    cell: ({ getValue }) => <AssetStatusBadge status={getValue()} />,
  }),
  columnHelper.accessor("currentHolder", {
    header: "Holder",
    cell: ({ getValue }) => getValue() ?? "—",
  }),
  columnHelper.accessor("createdAt", {
    header: "Created",
    cell: ({ getValue }) => dateFormatter.format(new Date(getValue())),
  }),
]);

const STATUS_FILTER_OPTIONS: Array<{
  label: string;
  value: AssetStatus | "all";
}> = [
  { label: "All statuses", value: "all" },
  { label: "Available", value: "Available" },
  { label: "In use", value: "InUse" },
  { label: "Maintenance", value: "Maintenance" },
  { label: "Retired", value: "Retired" },
];

type AssetsTableProps = {
  assets: AssetSummaryDto[];
};

export const AssetsTable = ({ assets }: AssetsTableProps) => {
  const [globalFilter, setGlobalFilter] = useState("");
  const [statusFilter, setStatusFilter] = useState<AssetStatus | "all">("all");

  const table = useTable({
    features,
    columns,
    data: assets,
    state: {
      globalFilter,
      columnFilters:
        statusFilter === "all" ? [] : [{ id: "status", value: statusFilter }],
    },
    onGlobalFilterChange: setGlobalFilter,
    globalFilterFn: "includesString",
    getColumnCanGlobalFilter: (column) =>
      column.id === "tag" || column.id === "name",
  });

  const rows = table.getRowModel().rows;
  const pagination = table.store.state.pagination;

  return (
    <div className="space-y-3">
      <div className="flex flex-wrap items-center gap-2">
        <Input
          placeholder="Search by name/tag"
          value={globalFilter}
          onChange={(event) => setGlobalFilter(event.target.value)}
          className="max-w-xs"
        />
        <select
          value={statusFilter}
          onChange={(event) =>
            setStatusFilter(event.target.value as AssetStatus | "all")
          }
          className="h-8 rounded-lg border border-input bg-transparent px-2.5 text-sm outline-none focus-visible:border-ring focus-visible:ring-3 focus-visible:ring-ring/50"
        >
          {STATUS_FILTER_OPTIONS.map((option) => (
            <option key={option.value} value={option.value}>
              {option.label}
            </option>
          ))}
        </select>
      </div>

      <div className="rounded-lg border">
        <Table>
          <TableHeader>
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id}>
                {headerGroup.headers.map((header) => (
                  <TableHead key={header.id}>
                    {header.isPlaceholder ? null : (
                      <table.FlexRender header={header} />
                    )}
                  </TableHead>
                ))}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody>
            {rows.length === 0 ? (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  className="h-24 text-center text-muted-foreground"
                >
                  No assets found.
                </TableCell>
              </TableRow>
            ) : (
              rows.map((row) => (
                <TableRow key={row.id}>
                  {row.getAllCells().map((cell) => (
                    <TableCell key={cell.id}>
                      <table.FlexRender cell={cell} />
                    </TableCell>
                  ))}
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </div>

      <div className="flex items-center justify-between">
        <span className="text-sm text-muted-foreground">
          Page {pagination.pageIndex + 1} of {Math.max(table.getPageCount(), 1)}
        </span>
        <div className="flex gap-2">
          <Button
            type="button"
            variant="outline"
            size="sm"
            onClick={() => table.previousPage()}
            disabled={!table.getCanPreviousPage()}
          >
            Previous
          </Button>
          <Button
            type="button"
            variant="outline"
            size="sm"
            onClick={() => table.nextPage()}
            disabled={!table.getCanNextPage()}
          >
            Next
          </Button>
        </div>
      </div>
    </div>
  );
};
