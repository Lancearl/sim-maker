## About

Modules are designed to be, as is perhaps obvious, modular. They can work together, but are designed to be fueled by input signals (and provide output signals to hook into).

All modules are connected by the MessageBroker which is a required class, or at least one implementation of a required class, for modules to receive inputs and send outputs.

The majority of module functionality is achieved by adding a scene to the game, this is also required for the MessageBroker which should be instantiated as a Autoload/Singleton.
