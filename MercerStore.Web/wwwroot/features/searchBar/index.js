import {createRoot} from 'react-dom/client';
import SearchBarApp from './SearchBarApp';

const searhBarRoot = document.getElementById('search-bar-root');

if (searhBarRoot) {
    const root = createRoot(searhBarRoot);
    root.render(
        <SearchBarApp/>
    );
}