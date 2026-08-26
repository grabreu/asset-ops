import { describe, expect, it } from "vitest";
import type { AssetSummaryDto } from "@/lib/api";
import { fireEvent, render, screen } from "@/testing/testUtils";
import { AssetsTable } from "./AssetsTable";

const asset = (overrides: Partial<AssetSummaryDto>): AssetSummaryDto => ({
  id: "1",
  tag: "AT-0001",
  name: "Sample Asset",
  status: "Available",
  currentHolder: null,
  createdAt: "2026-01-01T00:00:00Z",
  ...overrides,
});

describe("AssetsTable", () => {
  it("renders a row per asset", () => {
    render(
      <AssetsTable
        assets={[
          asset({ id: "1", tag: "AT-0001", name: "Laptop" }),
          asset({ id: "2", tag: "AT-0002", name: "Camera" }),
        ]}
      />,
    );

    expect(screen.getByText("AT-0001")).toBeInTheDocument();
    expect(screen.getByText("Laptop")).toBeInTheDocument();
    expect(screen.getByText("AT-0002")).toBeInTheDocument();
    expect(screen.getByText("Camera")).toBeInTheDocument();
  });

  it("filters by tag or name and shows an empty state when nothing matches", () => {
    render(
      <AssetsTable
        assets={[
          asset({ id: "1", tag: "AT-0001", name: "Laptop" }),
          asset({ id: "2", tag: "AT-0002", name: "Camera" }),
        ]}
      />,
    );

    const search = screen.getByPlaceholderText("Search by name/tag");

    fireEvent.change(search, { target: { value: "camera" } });
    expect(screen.queryByText("AT-0001")).not.toBeInTheDocument();
    expect(screen.getByText("AT-0002")).toBeInTheDocument();

    fireEvent.change(search, { target: { value: "no such asset" } });
    expect(screen.getByText("No assets found.")).toBeInTheDocument();
  });

  it("filters by status", () => {
    render(
      <AssetsTable
        assets={[
          asset({ id: "1", tag: "AT-0001", status: "Available" }),
          asset({ id: "2", tag: "AT-0002", status: "InUse" }),
        ]}
      />,
    );

    fireEvent.change(screen.getByDisplayValue("All statuses"), {
      target: { value: "InUse" },
    });

    expect(screen.queryByText("AT-0001")).not.toBeInTheDocument();
    expect(screen.getByText("AT-0002")).toBeInTheDocument();
  });

  it("paginates when there are more than 10 assets", () => {
    const assets = Array.from({ length: 12 }, (_, index) =>
      asset({
        id: String(index + 1),
        tag: `AT-${String(index + 1).padStart(4, "0")}`,
      }),
    );

    render(<AssetsTable assets={assets} />);

    expect(screen.getByText("Page 1 of 2")).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Previous" })).toBeDisabled();
    expect(screen.getByText("AT-0001")).toBeInTheDocument();
    expect(screen.queryByText("AT-0011")).not.toBeInTheDocument();

    fireEvent.click(screen.getByRole("button", { name: "Next" }));

    expect(screen.getByText("Page 2 of 2")).toBeInTheDocument();
    expect(screen.getByRole("button", { name: "Next" })).toBeDisabled();
    expect(screen.getByText("AT-0011")).toBeInTheDocument();
    expect(screen.queryByText("AT-0001")).not.toBeInTheDocument();
  });
});
