import { createFileRoute, Outlet } from "@tanstack/react-router";
import { SiteHeader } from "@/components/layout/SiteHeader";

const RouteComponent = () => {
  return (
    <div className="flex min-h-svh flex-col">
      <SiteHeader />
      <main className="flex-1 p-4">
        <Outlet />
      </main>
    </div>
  );
};

export const Route = createFileRoute("/_app")({
  component: RouteComponent,
});
