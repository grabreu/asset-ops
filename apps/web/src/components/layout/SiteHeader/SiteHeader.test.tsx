import { describe, expect, it } from "vitest";
import { render, screen } from "@/testing/testUtils";
import { SiteHeader } from "./SiteHeader";

describe("SiteHeader", () => {
  it("renders the brand and theme toggle", () => {
    render(<SiteHeader />);

    expect(screen.getByText("AssetOps")).toBeInTheDocument();
    expect(
      screen.getByRole("button", { name: "Toggle theme" }),
    ).toBeInTheDocument();
  });
});
