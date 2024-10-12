import {createTheme} from "@mantine/core";

export const theme = createTheme({
    components: {
        Button: {
            defaultProps: {
                loaderProps: {type: "bars", size: 'xs'},
            }
        }
    },
    focusRing: "auto",
    fontSmoothing: true,
    white: "#feffff",
    black: "#1b1a1a",
    defaultGradient: {from: 'blue.7', to: 'gray.4', deg: 110},
    primaryColor: "blue",
    primaryShade: 9,
    defaultRadius: "xs",
    respectReducedMotion: true,
    shadows: {
        md: '1px 1px 3px rgba(0, 0, 0, .10)',
        lg: '1px 1px 3px rgba(0, 0, 0, .15)',
        xl: '5px 5px 3px rgba(0, 0, 0, .20)',
    },
    headings: {
        fontFamily: 'Figtree, sans-serif',
        fontWeight: "700",
        textWrap: "balance",
    },
    fontFamily: '"Afacad Flux", system-ui',
});

