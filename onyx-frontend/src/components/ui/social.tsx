import InstagramIcon from "@/assets/images/icons/ins.svg";
import FacebookIcon from "@/assets/images/icons/fb.svg";
import TwitterIcon from "@/assets/images/icons/tw.svg";

const social = () => {
    return (
      <div className="flex justify-between w-24 flex-row">   
        <div className="cursor-pointer">
            <img src={FacebookIcon} alt="Facebook Icon"/>
        </div>
        <div className="cursor-pointer">
            <img src={TwitterIcon} alt="Twitter Icon"/>
        </div>
        <div className="cursor-pointer">
            <img src={InstagramIcon} alt="Instagram Icon"/>
        </div>
    </div>  
    );
  };
  export default social;