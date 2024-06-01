import { createFileRoute } from "@tanstack/react-router";



import Hero from "@/components/home/Hero";
import Navbar from "@/components/home/Navbar";

export const Route = createFileRoute("/_home-layout")({
  component: () => (
    <div className="max-w-1440px md:w-full lg:max-w-1440px xl:w-1440px max-h-700px h-auto mx-auto overflow-hidden bg-background">
      <Navbar/>
      <Hero />
    </div>
  ),
});
