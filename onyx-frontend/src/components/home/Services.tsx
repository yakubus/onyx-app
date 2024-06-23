import { Button } from "@/components/ui/button";
import IconCard from "@/assets/images/icons/black-logo.png";
const Services = () => {
    return (
        <div className="h-auto max-w-1440px w-full bg-white w-full">
            <div className="h-auto max-w-1440px  w-full bg-white py-16 relative flex justify-between flex-wrap flex-col md:flex-row">
                <div className="min-h-52 w-11/12 md:w-30% border-2 p-6 mx-auto md:mx-0 mb-6 md:mb-0 bg-card-foreground">
                    <div>
                        <img src={IconCard} alt="Icon card"/>
                    </div>
                    <p className="text-lg font-semibold mt-4 text-card">What is Cryptocurrency?</p>
                    <p className="mt-6 text-card">If you're newcomer in the world of cryptocurrency, our experts will introduce you to the basic features...</p>
                    <Button className="mt-4">READ MORE</Button>
                </div>                
                <div className="min-h-52 w-11/12 md:w-30% border-2 p-6 mx-auto md:mx-0 mb-6 md:mb-0 bg-secondary">
                    <div>
                        <img src={IconCard} alt="Icon card"/>
                    </div>
                    <p className="text-lg font-semibold mt-4">Cryptocurrency Space</p>
                    <p className="mt-6">Here you will find the basic information about successful investments and how to create the best portfolio...</p>
                    <Button className="mt-4">READ MORE</Button>
                </div>
                <div className="min-h-52 w-11/12 md:w-30% border-2 p-6 mx-auto md:mx-0  mb-6 md:mb-0 bg-card-foreground">
                    <div>
                        <img src={IconCard} alt="Icon card"/>
                    </div>
                    <p className="text-lg font-semibold mt-4 text-card">Investment Portfolio</p>
                    <p className="mt-4 text-card">We keep you posted about the most recent updates in the world of cryptocurrency. Learn more about...</p>
                    <Button className="mt-6">READ MORE</Button>
                </div>
            </div>
      </div>
    );
};
export default Services;