import { ReactFlowInstance } from 'reactflow';

export  interface ExtendedWindow extends Window {
    flowInstance: ReactFlowInstance | null;
}
