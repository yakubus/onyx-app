import { Button } from "./components/ui/button";

function App() {
  return (
    <div className="mx-6 space-y-4 p-10">
      <h1 className="text-6xl font-black">This is hero heading</h1>
      <h2 className="text-2xl font-semibold">This is h2</h2>
      <p className="text-muted-foreground">
        Lorem ipsum dolor sit amet consectetur adipisicing elit. Magnam facilis
        quam ratione est. Voluptate delectus enim aliquid quasi nemo odit.
      </p>
      <Button>Primary Button</Button>
      <Button variant="secondary">Secondary Button</Button>
      <Button variant="outline">Secondary Button</Button>
    </div>
  );
}

export default App;
