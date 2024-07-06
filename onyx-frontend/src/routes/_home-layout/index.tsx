import { createFileRoute } from "@tanstack/react-router";
import Pricing from "@/components/home/Pricing";
import About from "@/components/home/About";
import LatestNews from "@/components/home/LatestNews";
import Services from "@/components/home/Services";
import Cta from "@/components/home/Cta";

export const Route = createFileRoute("/_home-layout/")({
  component: () => (
    <div className="max-w-1440px lg:max-w-1440px h-auto mx-auto overflow-hidden bg-background md:w-full xl:w-1440px">
      <Services />
      <About />      
      <Cta />      
      <LatestNews />
      <Pricing />      
    </div>
  ),
});
