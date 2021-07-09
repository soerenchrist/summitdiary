import {
  Chart,
  PieController,
  ArcElement,
  LineController,
  LineElement,
  PointElement,
  Title,
  Legend,
  CategoryScale,
  LinearScale,
  Tooltip,
  BarController,
  BarElement,
} from 'chart.js';

export default () => {
  Chart.register(
    PieController,
    ArcElement,
    LineController,
    BarController,
    LineElement,
    BarElement,
    PointElement,
    CategoryScale,
    LinearScale,
    Title,
    Legend,
    Tooltip,
  );
};
