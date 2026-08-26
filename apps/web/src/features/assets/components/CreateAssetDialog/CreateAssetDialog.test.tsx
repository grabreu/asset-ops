import { beforeEach, describe, expect, it, vi } from "vitest";
import type { AssetSummaryDto } from "@/lib/api";
import * as sdk from "@/lib/api/sdk.gen";
import { fireEvent, render, screen } from "@/testing/testUtils";
import { CreateAssetDialog } from "./CreateAssetDialog";

vi.mock("@/lib/api/sdk.gen", async (importOriginal) => {
  const actual = await importOriginal<typeof import("@/lib/api/sdk.gen")>();
  return {
    ...actual,
    createAsset: vi.fn(),
  };
});

const createAssetMock = vi.mocked(sdk.createAsset);

const sampleAsset: AssetSummaryDto = {
  id: "1",
  tag: "AT-0001",
  name: "Sample Asset",
  status: "Available",
  currentHolder: null,
  createdAt: "2026-01-01T00:00:00Z",
};

const openDialog = () => {
  fireEvent.click(screen.getByRole("button", { name: "New asset" }));
};

describe("CreateAssetDialog", () => {
  beforeEach(() => {
    createAssetMock.mockReset();
  });

  it("opens the dialog with empty fields when the trigger is clicked", () => {
    render(<CreateAssetDialog />);

    openDialog();

    expect(screen.getByRole("dialog")).toBeInTheDocument();
    expect(screen.getByLabelText("Tag")).toHaveValue("");
    expect(screen.getByLabelText("Name")).toHaveValue("");
  });

  it("shows validation errors when submitting empty fields and does not call the API", async () => {
    render(<CreateAssetDialog />);
    openDialog();

    fireEvent.click(screen.getByRole("button", { name: "Create" }));

    expect(await screen.findByText("Tag is required.")).toBeInTheDocument();
    expect(screen.getByText("Name is required.")).toBeInTheDocument();
    expect(createAssetMock).not.toHaveBeenCalled();
  });

  it("submits trimmed values and closes the dialog on success", async () => {
    createAssetMock.mockResolvedValue({ data: sampleAsset } as never);

    render(<CreateAssetDialog />);
    openDialog();

    fireEvent.change(screen.getByLabelText("Tag"), {
      target: { value: "  AT-0001  " },
    });
    fireEvent.change(screen.getByLabelText("Name"), {
      target: { value: "  Sample Asset  " },
    });

    fireEvent.click(screen.getByRole("button", { name: "Create" }));

    await screen.findByText("New asset", { selector: "button" });

    expect(createAssetMock).toHaveBeenCalledWith(
      expect.objectContaining({
        body: { tag: "AT-0001", name: "Sample Asset" },
      }),
    );
    expect(screen.queryByRole("dialog")).not.toBeInTheDocument();
  });

  it("shows the conflict detail from the API and keeps the dialog open", async () => {
    createAssetMock.mockRejectedValue({
      status: 409,
      detail: "An asset with tag 'AT-0001' already exists.",
    });

    render(<CreateAssetDialog />);
    openDialog();

    fireEvent.change(screen.getByLabelText("Tag"), {
      target: { value: "AT-0001" },
    });
    fireEvent.change(screen.getByLabelText("Name"), {
      target: { value: "Sample Asset" },
    });

    fireEvent.click(screen.getByRole("button", { name: "Create" }));

    expect(
      await screen.findByText("An asset with tag 'AT-0001' already exists."),
    ).toBeInTheDocument();
    expect(screen.getByRole("dialog")).toBeInTheDocument();
  });

  it("resets the form after cancelling and reopening", () => {
    render(<CreateAssetDialog />);
    openDialog();

    fireEvent.change(screen.getByLabelText("Tag"), {
      target: { value: "AT-0001" },
    });
    fireEvent.click(screen.getByRole("button", { name: "Cancel" }));

    openDialog();

    expect(screen.getByLabelText("Tag")).toHaveValue("");
  });
});
