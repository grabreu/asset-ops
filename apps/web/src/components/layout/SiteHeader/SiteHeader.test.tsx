import { describe, expect, it } from "vitest";
import { render, screen } from "@/testing/testUtils";
import { SiteHeader } from "./SiteHeader";

describe("SiteHeader", () => {
  it("renders the brand, search, and primary actions", () => {
    render(<SiteHeader />);

    expect(screen.getByText("AssetOps")).toBeInTheDocument();
    expect(
      screen.getByPlaceholderText("Search by name/tag"),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: "New asset" }),
    ).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: "Toggle theme" }),
    ).toBeInTheDocument();
  });
});
