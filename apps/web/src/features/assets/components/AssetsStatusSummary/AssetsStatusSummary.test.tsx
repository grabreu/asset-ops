import { describe, expect, it } from "vitest";
import type { AssetSummaryDto } from "@/lib/api";
import { render, screen } from "@/testing/testUtils";
import { AssetsStatusSummary } from "./AssetsStatusSummary";

const asset = (overrides: Partial<AssetSummaryDto>): AssetSummaryDto => ({
  id: "1",
  tag: "AT-0001",
  name: "Sample Asset",
  status: "Available",
  currentHolder: null,
  createdAt: "2026-01-01T00:00:00Z",
  ...overrides,
});

describe("AssetsStatusSummary", () => {
  it("counts assets per status", () => {
    render(
      <AssetsStatusSummary
        assets={[
          asset({ id: "1", status: "Available" }),
          asset({ id: "2", status: "Available" }),
          asset({ id: "3", status: "InUse" }),
          asset({ id: "4", status: "Maintenance" }),
        ]}
      />,
    );

    expect(screen.getByText("Available").nextSibling).toHaveTextContent("2");
    expect(screen.getByText("In use").nextSibling).toHaveTextContent("1");
    expect(screen.getByText("Maintenance").nextSibling).toHaveTextContent("1");
    expect(screen.getByText("Retired").nextSibling).toHaveTextContent("0");
  });

  it("renders zero for every status when there are no assets", () => {
    render(<AssetsStatusSummary assets={[]} />);

    for (const label of ["Available", "In use", "Maintenance", "Retired"]) {
      expect(screen.getByText(label).nextSibling).toHaveTextContent("0");
    }
  });
});
