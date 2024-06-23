import Social from "@/components/ui/social";
import PhoneIcon from "@/assets/images/icons/phone.svg";
import EmailIcon from "@/assets/images/icons/email.svg";
import LocationIcon from "@/assets/images/icons/location.svg";
import Brand from "@/components/Logo";


const Footer = () => {
  return (
    <div className="flex max-w-1440px h-auto bg-background flex-col mx-auto mt-8 p-4">
      <div className="w-full h-auto flex flex-col md:flex-row pb-8 justify-start md:justify-center items-center">
        <div className="md:w-1/4">
          <div className="mt-4 flex ">
            <Brand className="text-foreground flex justify-start"/>
          </div>          
          <div className="mt-8 md:mt-4 mb-2 flex flex-row">
                <div className="location flex justify-center items-center mr-4">
                   <img src={LocationIcon} alt="Location Icon" style={{height: '24px', width: '24px'}}/> 
                </div>
                <div className="">Lorem ipsum dolor sit amet, consectetur adipiscing elit</div>
            </div>
            <div className="phone mt-4 flex flex-row">
            <div className="flex justify-center items-center mr-4"><img src={PhoneIcon} alt="Phone Icon"/></div>
                <div className="">+1-543-123-4567</div>
            </div>
            <div className="email mt-2 flex flex-row">
                <div className="flex justify-center items-center mr-4"><img src={EmailIcon} alt="Email Icon"/></div>
                <div className="">example@onyx.com</div>
            </div>
          </div>
        <div className="w-full md:w-1/4 flex justify-center items-center mt-8 md:mt-0">
          <ul className=" mt-4 flex flex-row md:flex-col gap-5 md:gap-0">
            <li className="cursor-pointer">About Us</li>
            <li className="cursor-pointer">Jobs</li>
            <li className="cursor-pointer">Press</li>
            <li className="cursor-pointer">Blog</li>
          </ul>
        </div>
        <div className="w-full md:w-1/4 flex justify-center items-center">
          <ul className="mt-4 flex flex-row md:flex-col gap-5 md:gap-0">
            <li className="cursor-pointer">Contact Us</li>
            <li className="cursor-pointer">Terms</li>
            <li className="cursor-pointer">Privacy</li>                           
          </ul>
        </div>
        <div className="w-full md:w-1/4 flex justify-center md:justify-center items-center mt-8 md:mt-0">
          <Social />
        </div> 
      </div>
      <div className="max-w-1440px h-auto flex justify-center items-center border-t border-solid border-primary-dark">
        <div className="h-auto w-auto text-foreground pb-2 pt-2">ONYX - 2024</div> 
      </div>     
    </div>
  );
};
export default Footer;