import { Button } from "../ui/button";

const Pricing = () => {
    return (
        <div className="h-auto max-w-1440px w-full bg-white w-full border-2 flex justify-start flex-col" >
            <div className="h-500px bg-gray-800 flex justify-start flex-col py-8 ">
                 <p className="text-3xl font-bold pt-8 mb-8 w-full text-center text-card ">Quality Business Services at Affordable Price</p>
                <p className="w-3/5 mx-auto block text-center text-card ">On the other hand we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment so blinded by desire.</p>
            </div>
           
            <div className="h-auto max-w-1440px w-full bg-white pb-16 flex justify-center flex-wrap flex-col md:flex-row gap-3 ">
                <div className="w-3/5 md:w-80 h-auto border-2 py-16 pl-4 pr-4 flex flex-col md:-mt-60 mt-4 mx-auto md:mx-0 md:justify-center">
                    <p className="text-center font-bold md:text-card sm:text-primary  ">Standard Plan</p>
                    <div className="text-center mt-16 mb-4 md:text-card sm:text-primary  ">
                        <span className="align-top">$</span> 
                        <span className="text-6xl font-bold">150</span>
                        <span>/month</span>
                    </div>
                    <p className="text-center my-2 mt-8">
                    Create excepteur sint occaecat cupidatat non proident
                    </p>
                    <ul className="mt-20 text-center">
                        <li className="h-16 border-b">Complete business set-up</li>
                        <li className="h-16  mt-8 border-b">Financial planning</li>
                        <li className="h-16  mt-8 border-b">Technology support</li>
                        <li className="h-16 mt-8">Infrastructure</li>
                    </ul>
                    <Button className="text-center mt-4">GET STARTED</Button>
                </div>
                <div className="w-3/5 md:w-80 h-auto border-2 py-16 pl-4 pr-4 flex flex-col md:-mt-60 mt-0 mx-auto md:mx-0 md:justify-center">
                    <p className="text-center font-bold md:text-card sm:text-primary  ">Economy Plan</p>
                    <div className="text-center mt-16 mb-4 md:text-card sm:text-primary  ">
                        <span className="align-top">$</span> 
                        <span className="text-6xl font-bold">300</span>
                        <span>/month</span>
                    </div>
                    <p className="text-center my-2 mt-8">
                    Create excepteur sint occaecat cupidatat non proident
                    </p>
                    <ul className="mt-20 text-center">
                        <li className="h-16 border-b">Complete business set-up</li>
                        <li className="h-16  mt-8 border-b">Financial planning</li>
                        <li className="h-16  mt-8 border-b">Technology support</li>
                        <li className="h-16 mt-8">Infrastructure</li>
                    </ul>
                    <Button className="text-center mt-4">GET STARTED</Button>
                </div>
                <div className="w-3/5 md:w-80 h-auto border-2 py-16 pl-4 pr-4 flex flex-col lg:-mt-60 mt-0 mx-auto md:mx-0 md:justify-center">
                    <p className="text-center font-bold text-card lg:text-card sm:text-primary  md:text-primary">Executive Plan</p>
                    <div className="text-center mt-16 mb-4 lg:text-card sm:text-primary  md:text-primary">
                        <span className="align-top">$</span> 
                        <span className="text-6xl font-bold">450</span>
                        <span>/month</span>
                    </div>
                    <p className="text-center my-2 mt-8">
                    Create excepteur sint occaecat cupidatat non proident
                    </p>
                    <ul className="mt-20 text-center">
                        <li className="h-16 border-b">Complete business set-up</li>
                        <li className="h-16  mt-8 border-b">Financial planning</li>
                        <li className="h-16  mt-8 border-b">Technology support</li>
                        <li className="h-16 mt-8">Infrastructure</li>
                    </ul>
                    <Button className="text-center mt-4">GET STARTED</Button>
                </div>
            </div>
      </div>
    );
};
export default Pricing;