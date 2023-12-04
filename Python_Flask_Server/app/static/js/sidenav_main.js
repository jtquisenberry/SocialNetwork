

        function openNav() {
            document.getElementById("mySidenav").style.width = "500px";
            document.getElementById("wrapper").style.marginLeft = "500px";
            document.getElementById("mySidenav").style.overflow = 'auto';
            document.getElementById("threes").style.visibility = "hidden";
            document.getElementById("status").style.visibility = "hidden";
        }

        function closeNav() {
            document.getElementById("mySidenav").style.width = "0";
            document.getElementById("wrapper").style.marginLeft = "0";
            document.getElementById("mySidenav").style.overflow = 'hidden';
            document.getElementById("threes").style.visibility = "visible";
            document.getElementById("status").style.visibility = "visible";
        }

        function toggleTop() {

            if (document.getElementsByClassName("navbar")[0].style.display == "none")
            {
                document.getElementsByClassName("navbar")[0].style.display = "block";
                document.getElementById("wrapper").style.height = "0%";
                setupGraph();
            }
            else
            {
                document.getElementsByClassName("navbar")[0].style.display = "none"
                document.getElementById("wrapper").style.height = "100%";
                //document.getElementById("tall").style.height = "100%";
                setupGraph();
            }



            //document.getElementsByClassName("navbar")[0].style.height = "0px";
        }

        function nightMode() {
            alert("night mode!");
        }


        //Register a listener for the end of the transition of wrapper.
        (function () {
            var e = document.getElementById('wrapper');

            function whichTransitionEvent() {
                var t;
                var el = document.createElement('fakeelement');
                var transitions = {
                    'transition': 'transitionend',
                    'OTransition': 'oTransitionEnd',
                    'MozTransition': 'transitionend',
                    'WebkitTransition': 'webkitTransitionEnd'
                }

                for (t in transitions) {
                    if (el.style[t] !== undefined) {
                        return transitions[t];
                    }
                }
            }

            var transitionEvent = whichTransitionEvent();
            transitionEvent && e.addEventListener(transitionEvent, function () {
                //alert('Transition complete!  This is the callback, no library needed!');
                setupGraph();
            });

            startFade = function () {
                e.className += ' hide';
            }
        })();


        window.addEventListener("resize", resizeWindow);

        function resizeWindow()
        {
            setupGraph();
        }



