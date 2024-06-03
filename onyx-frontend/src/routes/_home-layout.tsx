import { createFileRoute } from "@tanstack/react-router";

import Hero from "@/components/home/Hero";
import Navbar from "@/components/home/Navbar";

export const Route = createFileRoute("/_home-layout")({
  component: () => (
    <div className="max-w-1440px lg:max-w-1440px max-h-700px mx-auto h-auto overflow-hidden bg-background md:w-full xl:w-1440px">
      <Navbar />
      <Hero />
    </div>
  ),
});
