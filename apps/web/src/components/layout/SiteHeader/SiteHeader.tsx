import { Boxes, Plus } from "lucide-react";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ModeToggle } from "@/components/ui/theme";

export const SiteHeader = () => {
  return (
    <header className="flex items-center gap-4 border-b px-4 py-2">
      <div className="flex items-center gap-2 font-medium">
        <Boxes className="size-5" />
        AssetOps
      </div>
      <Input placeholder="Search by name/tag" className="max-w-xs" />
      <div className="flex-1" />
      <Button size="sm">
        <Plus data-icon="inline-start" />
        New asset
      </Button>
      <ModeToggle />
    </header>
  );
};
