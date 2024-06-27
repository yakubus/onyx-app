import { useEffect, useRef } from "react";

export const useCarouselKeyboardDisable = () => {
  const emblaRef = useRef(null);

  useEffect(() => {
    const handleKeydown = (event: KeyboardEvent) => {
      const activeElement = document.activeElement;
      if (
        activeElement &&
        (activeElement.tagName === "INPUT" ||
          activeElement.tagName === "TEXTAREA")
      ) {
        if (event.key === "ArrowLeft" || event.key === "ArrowRight") {
          // Prevent default action of moving the carousel
          event.stopPropagation();
        }
      }
    };

    // Attach the event listener to the document
    document.addEventListener("keydown", handleKeydown, true);

    return () => {
      // Clean up the event listener on component unmount
      document.removeEventListener("keydown", handleKeydown, true);
    };
  }, []);

  return emblaRef;
};
