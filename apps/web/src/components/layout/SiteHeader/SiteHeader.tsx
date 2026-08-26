import { Boxes } from "lucide-react";
import { ModeToggle } from "@/components/ui/theme";

export const SiteHeader = () => {
  return (
    <header className="flex items-center gap-4 border-b px-4 py-2">
      <div className="flex items-center gap-2 font-medium">
        <Boxes className="size-5" />
        AssetOps
      </div>
      <div className="flex-1" />
      <ModeToggle />
    </header>
  );
};
