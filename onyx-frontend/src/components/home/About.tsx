import { Button } from "@/components/ui/button";
import AboutImg from "@/assets/images/laptop.png";
const About = () => {
    return (
        <div className="h-auto max-w-1440px w-full bg-white w-full">
            <div className="h-auto max-w-1440px  w-full bg-white relative flex justify-between flex-wrap flex-col md:flex-row">
                <div className="min-h-auto w-full md:w-2/4 py-16 pl-6 pr-16 mx-auto md:mx-0 mb-6 md:mb-0">
                    <p className="text-3xl font-bold mb-16">Financia Provides that Extensive Reach Among your Prospects</p>
                    <p className="">Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam quis nostrud and trouble that are bound to ensue and equal blame belongs to those.</p>
                    <p className="mt-6">On the other hand we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment so blinded by desire that they cannot foresee the pain and trouble that are bound to ensue and equal blame belongs to those.
                    </p>
                    <Button className="mt-14">READ MORE</Button>
                </div>                
                <div className="min-h-auto w-full md:w-2/4 p-6 flex justify-center items-center  md:mx-0 mb-6 md:mb-0">
                    <img src={AboutImg} alt="About img" className="h-fit"/>
                </div>                
            </div>
      </div>
    );
};
export default About;