import { Button } from "@/components/ui/button";
import HeroImageBackground from "@/assets/images/hero/hero-bg.png";
import HeroImage from "@//assets/images/hero/hero-img.svg";

const Hero = () => {
    return (
        <div className="container max-w-full lg:h-screen lg:max-w-1440px p-0 m-0 ml-0 mx-auto lg:ml-122px mt-28 mb-10">
        <div className="grid grid-cols-1 h-auto lg:grid-cols-2">
            <div className="flex justify-center flex-col lg:max-w-600px md:pt-10 lg:pt-0 max-w-full h:auto px-10 sm:px-30 lg:px-0">
                <h1 className="font-bold leading-snug text-4xl sm:text-6xl w-full sm:w-10/12 sm:mx-auto lg:mx-0 lg:w-498px text-foreground text-center lg:text-left">The only budget planner you'll ever need.</h1>
                <p className="mt-10 lg:w-386px text-center text-foreground lg:text-left w-full sm:w-10/12 sm:mx-auto lg:mx-0">Lorem ipsum dolor sit amet consectetur adipisicing elit. Quae perferendis labore quibusdam mollitia quaerat maiores, iste reiciendis laudantium laboriosam eveniet!</p>
                <Button className="w-56 h-16 rounded-full mt-16 font-semibold text-base mx-auto lg:mx-0">Get started</Button>
            </div>
            <div className="flex justify-center items-center w-full h-518px lg:w-990px bottom-0 lg:-bottom-20 xl:-bottom-24 lg:right-56 xl:right-80 right-0 relative">
                <img className="w-full sm:w-auto md:h-680px lg:h-auto -bottom-28 md:right-0 right-0 xl:w-990px absolute z-10" src={HeroImageBackground} alt="Hero image background" />
                <img className="h-full absolute -bottom-28 right-0 lg:right-64 xl:right-40 md:-bottom-28 lg:bottom-24 xl:bottom-20 lg:h-600px z-20" src={HeroImage} alt="Hero Image" />          
            </div>
        </div>
      </div>
    );
};
export default Hero;